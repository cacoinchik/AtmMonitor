<script setup>
import { ref, onMounted, computed } from 'vue';
import { atmApi } from './services/api';

// State
const atms = ref([]);
const transactions = ref([]);
const loading = ref(false);
const error = ref(null);

// Фильтры
const allATMsSelected = ref(true);
const selectedATMIds = ref([]);
const dateFrom = ref('2024-11-01');
const dateTo = ref('2024-12-31');

// Computed
const isMultiselectDisabled = computed(() => allATMsSelected.value);
const isAllCheckboxDisabled = computed(() => selectedATMIds.value.length > 0);

// Загрузка банкоматов при монтировании
onMounted(async () => {
  try {
    loading.value = true;
    atms.value = await atmApi.getATMs();
    await loadTransactions();
  } catch (err) {
    error.value = 'Ошибка загрузки данных: ' + err.message;
    console.error(err);
  } finally {
    loading.value = false;
  }
});

// Загрузка транзакций
async function loadTransactions() {
  try {
    loading.value = true;
    error.value = null;
    transactions.value = null;
    
    const filters = {
      atmIds: allATMsSelected.value ? [] : selectedATMIds.value,
      dateFrom: dateFrom.value,
      dateTo: dateTo.value
    };
    
    const result = await atmApi.getTransactions(filters);
    
    if (Array.isArray(result)) {
      transactions.value = result;
    } else {
      console.error('❌ Некорректный формат:', result);
      transactions.value = [];
      error.value = 'Некорректный формат данных от сервера';
    }
  } catch (err) {
    console.error('❌ Ошибка:', err);
    error.value = 'Ошибка загрузки транзакций: ' + err.message;
    transactions.value = [];
  } finally {
    loading.value = false;
  }
}

// Обработчик изменения "Все банкоматы"
function handleAllATMsChange() {
  if (allATMsSelected.value) {
    selectedATMIds.value = [];
  }
}

// Обработчик изменения выбранных банкоматов
function handleATMSelectionChange() {
  if (selectedATMIds.value.length > 0) {
    allATMsSelected.value = false;
  }
}

// Форматирование даты
function formatDate(dateString) {
  const date = new Date(dateString);
  return date.toLocaleString('ru-RU', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
}

// Форматирование суммы
function formatAmount(amount) {
  return new Intl.NumberFormat('ru-RU').format(amount) + ' ₽';
}
</script>

<template>
  <div class="container mt-4">
    <h1 class="text-center mb-4">ATM Transactions Monitor</h1>

    <!-- Фильтры -->
    <div class="card mb-4">
      <div class="card-body">
        <h5 class="card-title">Фильтры</h5>
        
        <!-- Чекбокс "Все банкоматы" -->
        <div class="form-check mb-3">
          <input 
            class="form-check-input" 
            type="checkbox" 
            id="allATMs"
            v-model="allATMsSelected"
            @change="handleAllATMsChange"
            :disabled="isAllCheckboxDisabled"
          >
          <label class="form-check-label fw-bold" for="allATMs">
            Все банкоматы
          </label>
        </div>

        <!-- Мультиселект банкоматов -->
        <div class="mb-3" v-if="atms.length > 0">
          <label class="form-label">Выберите банкоматы:</label>
          <select 
            multiple 
            class="form-select" 
            v-model="selectedATMIds"
            @change="handleATMSelectionChange"
            :disabled="isMultiselectDisabled"
            size="5"
          >
            <option v-for="atm in atms" :key="atm.id" :value="atm.id">
              {{ atm.address }}
            </option>
          </select>
          <div class="form-text">Удерживайте Ctrl для выбора нескольких</div>
        </div><!-- Период -->
        <div class="row">
          <div class="col-md-4">
            <label class="form-label">С:</label>
            <input type="date" class="form-control" v-model="dateFrom">
          </div>
          <div class="col-md-4">
            <label class="form-label">По:</label>
            <input type="date" class="form-control" v-model="dateTo">
          </div>
          <div class="col-md-4 d-flex align-items-end">
            <button 
              class="btn btn-primary w-100" 
              @click="loadTransactions"
              :disabled="loading"
            >
              {{ loading ? 'Загрузка...' : 'Показать' }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Ошибка -->
    <div v-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <!-- Таблица транзакций -->
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">
          Результаты: {{ transactions ? transactions.length : 0 }} транзакций
        </h5>
        
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border" role="status">
            <span class="visually-hidden">Загрузка...</span>
          </div>
        </div>

        <div v-else-if="!transactions || transactions.length === 0" class="text-center py-5 text-muted">
          Транзакции не найдены
        </div>

        <div v-else-if="transactions && transactions.length > 0" class="table-responsive">
          <table class="table table-striped table-hover">
            <thead>
              <tr>
                <th>Дата и время</th>
                <th>Адрес банкомата</th>
                <th>Операция</th>
                <th class="text-end">Сумма</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="transaction in transactions" :key="transaction.id">
                <td>{{ formatDate(transaction.operationDate) }}</td>
                <td>{{ transaction.atmAddress }}</td>
                <td>
                  <span 
                    class="badge"
                    :class="transaction.type.includes('Снятие') ? 'bg-danger' : 'bg-success'"
                  >
                    {{ transaction.type }}
                  </span>
                </td>
                <td class="text-end fw-bold">{{ formatAmount(transaction.amount) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.form-select[disabled] {
  background-color: #e9ecef;
  cursor: not-allowed;
}

.form-check-input[disabled] {
  cursor: not-allowed;
}

.table th {
  background-color: #f8f9fa;
}
</style>
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./

RUN npm ci

COPY . .

ENV VITE_API_URL=http://localhost:5201/api

RUN npm run build

FROM nginx:alpine AS production
WORKDIR /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=build /app/dist .

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
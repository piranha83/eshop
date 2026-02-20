# Stage 1: Сборка приложения
FROM node:lts-alpine AS build-stage
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
COPY /src/scripts/maplibre-gl.js .
RUN npm run build --configuration=production

# Stage 2: Запуск nginx
FROM nginx:alpine
COPY --from=build-stage /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

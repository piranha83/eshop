FROM node:lts-alpine

WORKDIR /app

COPY package.json .
RUN npm install

COPY products.json .
COPY server.js .
COPY ./public /app/public/

EXPOSE 3000

CMD ["node", "server.js"]

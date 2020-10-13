FROM node:12.7-alpine
WORKDIR /usr/src/app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build
### STAGE 2: Run ###
FROM nginx:1.17.1-alpine
COPY --from=0 /usr/src/app/dist/web-app /usr/share/nginx/html
#package-lock.json

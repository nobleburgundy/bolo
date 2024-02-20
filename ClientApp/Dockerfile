# Use the official image as a base
FROM node:20 AS build

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the entire project
COPY . .

# Build the Angular app
RUN npm run build --prod

# Use nginx to serve the Angular app
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html

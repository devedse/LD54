# Use the latest nginx image as the base
FROM nginx:latest

# Copy the WebGL build to the default nginx public folder
COPY ./build/WebGL/* /etc/nginx/html

# Copy the custom nginx configuration to the default config location in nginx
COPY ./Docker/nginx.conf /etc/nginx/conf.d/default.conf

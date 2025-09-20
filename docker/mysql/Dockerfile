FROM mysql:8.4

COPY init/ /docker-entrypoint-initdb.d/

ENV MYSQL_ROOT_PASSWORD=root
ENV MYSQL_DATABASE=smart_mechanical_workshop
ENV MYSQL_USER=app_user
ENV MYSQL_PASSWORD=app_password

EXPOSE 3306

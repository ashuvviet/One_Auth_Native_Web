# create databases
CREATE DATABASE IF NOT EXISTS `readviewServicedb`;
CREATE DATABASE IF NOT EXISTS `rolesdb`;
CREATE DATABASE IF NOT EXISTS `dataServicedb`;
CREATE DATABASE IF NOT EXISTS `operation`;
CREATE DATABASE IF NOT EXISTS `keycloak`;

# create root user and grant rights
GRANT ALL PRIVILEGES ON *.* TO 'root'@'%';
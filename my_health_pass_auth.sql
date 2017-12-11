-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: my_health_pass_auth
-- ------------------------------------------------------
-- Server version	5.7.20-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `authentication_log`
--

DROP TABLE IF EXISTS `authentication_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `authentication_log` (
  `authentication_log_id` int(11) NOT NULL AUTO_INCREMENT,
  `ip_address` varchar(45) DEFAULT NULL,
  `request_data` blob,
  `user_agent` varchar(255) DEFAULT NULL,
  `insert_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `result_message` varchar(100) DEFAULT NULL,
  `authentication_logcol` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`authentication_log_id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `authentication_log`
--

LOCK TABLES `authentication_log` WRITE;
/*!40000 ALTER TABLE `authentication_log` DISABLE KEYS */;
INSERT INTO `authentication_log` VALUES (1,'useragent','requestdata','login','2017-12-10 23:58:48','success',NULL),(2,'useragent','requestdata','login','2017-12-11 11:23:29','success',NULL),(3,'useragent','requestdata','login','2017-12-11 11:36:49','success',NULL),(4,'useragent','requestdata','login','2017-12-11 11:45:04','success',NULL),(5,'useragent','requestdata','login','2017-12-11 11:52:54','success',NULL),(6,'useragent','requestdata','login','2017-12-11 11:53:25','success',NULL),(7,'useragent','requestdata','login','2017-12-11 12:00:42','Incorrect Password',NULL),(8,'useragent','requestdata','login','2017-12-11 12:00:42','success',NULL),(9,'useragent','requestdata','login','2017-12-11 12:01:07','Incorrect Password',NULL),(10,'useragent','requestdata','login','2017-12-11 12:01:07','success',NULL),(11,'useragent','requestdata','login','2017-12-11 12:17:40','Incorrect Password',NULL),(12,'useragent','requestdata','login','2017-12-11 12:17:40','success',NULL),(13,'useragent','requestdata','login','2017-12-11 12:20:22','Incorrect Password',NULL),(14,'useragent','requestdata','login','2017-12-11 12:20:22','success',NULL);
/*!40000 ALTER TABLE `authentication_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `authorization_config`
--

DROP TABLE IF EXISTS `authorization_config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `authorization_config` (
  `authorizatoin_config_id` int(11) NOT NULL AUTO_INCREMENT,
  `password_length_max` int(11) DEFAULT NULL,
  `password_length_min` int(11) DEFAULT NULL,
  `password_allowed_uppercase_count` int(11) DEFAULT NULL,
  `password_allowed_lowercase_count` int(11) DEFAULT NULL,
  `password_allowed_digit_count` int(11) DEFAULT NULL,
  `password_allowed_special_char_count` int(11) DEFAULT NULL,
  `max_user_session_seconds` int(11) DEFAULT NULL,
  `login_account_lock_attempts` int(11) DEFAULT NULL,
  `brute_force_block_attempts` int(11) DEFAULT NULL,
  `brute_force_identification_seconds` int(11) DEFAULT NULL,
  `brute_force_block_seconds` int(11) DEFAULT NULL,
  `location_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`authorizatoin_config_id`),
  KEY `location_id_idx` (`location_id`),
  CONSTRAINT `auth_location_id` FOREIGN KEY (`location_id`) REFERENCES `location` (`location_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `authorization_config`
--

LOCK TABLES `authorization_config` WRITE;
/*!40000 ALTER TABLE `authorization_config` DISABLE KEYS */;
INSERT INTO `authorization_config` VALUES (1,10,1,1,1,1,1,600,3,13,600,600,1);
/*!40000 ALTER TABLE `authorization_config` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `blacklist_log`
--

DROP TABLE IF EXISTS `blacklist_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `blacklist_log` (
  `blacklist_id` int(11) NOT NULL AUTO_INCREMENT,
  `ip_address` varchar(45) DEFAULT NULL,
  `user_agent` varchar(45) DEFAULT NULL,
  `flag_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`blacklist_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `blacklist_log`
--

LOCK TABLES `blacklist_log` WRITE;
/*!40000 ALTER TABLE `blacklist_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `blacklist_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `location`
--

DROP TABLE IF EXISTS `location`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `location` (
  `location_id` int(11) NOT NULL AUTO_INCREMENT,
  `country` varchar(45) DEFAULT NULL,
  `region` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`location_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `location`
--

LOCK TABLES `location` WRITE;
/*!40000 ALTER TABLE `location` DISABLE KEYS */;
INSERT INTO `location` VALUES (1,'Trinidad','Chaguanas'),(2,'Trnidad','Arima'),(3,'United Kingdom','London');
/*!40000 ALTER TABLE `location` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `failed_login_attempts` int(11) NOT NULL DEFAULT '0',
  `failed_login_date_time` datetime DEFAULT NULL,
  `location_id` int(11) NOT NULL,
  `account_locked` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`user_id`),
  KEY `location_id_idx` (`location_id`),
  CONSTRAINT `user_location_id` FOREIGN KEY (`location_id`) REFERENCES `location` (`location_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'keshav','password',0,'2017-12-11 12:20:22',1,0);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-12-11 14:51:43

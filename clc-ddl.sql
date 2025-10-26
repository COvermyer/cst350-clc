-- MySQL Workbench Synchronization
-- Generated: 2025-10-08 23:05
-- Model: CST-350 CLC Project
-- Version: 1.0
-- Project: Name of the project
-- Author: Caleb

CREATE DATABASE IF NOT EXISTS `minesweeperapp` DEFAULT CHARACTER SET utf8;
USE `minesweeperapp`;

CREATE TABLE IF NOT EXISTS `minesweeperapp`.`users` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(100) NOT NULL,
  `passwordHash` VARCHAR(300) NOT NULL,
  `saltHexString` VARCHAR(300) NOT NULL,
  `groups` VARCHAR(255) NULL DEFAULT NULL,
  `first_name` VARCHAR(45) NULL DEFAULT NULL,
  `last_name` VARCHAR(45) NULL DEFAULT NULL,
  `age` INT(11) NULL DEFAULT NULL,
  `email` VARCHAR(100) NULL DEFAULT NULL,
  `state` VARCHAR(30) NULL DEFAULT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `username_UNIQUE` (`username` ASC),
  UNIQUE INDEX `email_UNIQUE` (`email` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS scores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NULL,
    difficulty TINYINT NOT NULL,
    score INT NOT NULL,
    time_taken INT NOT NULL,
    played_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE INDEX IF NOT EXISTS idx_scores_user
    ON scores(username, difficulty, score DESC, time_taken ASC, played_at DESC);
CREATE INDEX IF NOT EXISTS idx_scores_top
    ON scores(score DESC, time_taken ASC, played_at DESC);


INSERT INTO `users` (username, passwordHash, saltHexString, groups, first_name, last_name, age, email, `state`) VALUES ('admin', '856C9D33F2AD3CAE9694E79E21A81DDDB32B98D54DC159EA648D57A4AC0B7DFFBECA4E59659CB7EADE8FE249A68F0B67D5FC51CC1C1175B422FDF6CDE6D64AD5', 'BF11CD1F14E5F45403F022CE6E022319C7E19B769981E1B1481CC64AD92777C78C53B8A5573672DC0920CFFABEA88F281EDC3D5E6E92E5D35D6A69D8EDBB524B', 'ADMIN,USER', 'admin', 'admin', 99, 'admin@minesweeper.net', 'AZ');
  
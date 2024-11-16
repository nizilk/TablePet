CREATE DATABASE tablepet;
USE tablepet;

CREATE TABLE CalendarEvents (
                                id INT AUTO_INCREMENT PRIMARY KEY,
                                start_time DATETIME NOT NULL,
                                description VARCHAR(255) NOT NULL
);
CREATE TABLE Alarms (
                        id INT PRIMARY KEY AUTO_INCREMENT,
                        time TIME NOT NULL,
                        status BOOLEAN NOT NULL,           
                        repeat_mode VARCHAR(20) NOT NULL,   
                        custom_days VARCHAR(100)        
);

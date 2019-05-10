 CREATE TABLE
    `Room` (
        `r_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `r_vId` INT(11) NOT NULL,   
        `r_Name` varchar(50) NOT NULL,
        `r_Description` varchar(50) NOT NULL,
        PRIMARY KEY(`r_Id`),
		FOREIGN KEY (`r_vId`)  REFERENCES `Villa` (`v_Id`)
    ) 
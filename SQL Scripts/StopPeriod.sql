 CREATE TABLE
    `StopPeriod` (
        `sp_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `sp_pId` INT(11) NOT NULL,  
        `sp_Start` datetime NOT NULL,   
        `sp_End` datetime not NULL,
        PRIMARY KEY(`sp_Id`),
		FOREIGN KEY (`sp_pId`)  REFERENCES `Package` (`p_Id`)
    ) 
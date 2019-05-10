 CREATE TABLE
    `AccommodationPrice` (
        `ap_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `ap_rId` INT(11) NOT NULL,   
        `ap_aId` INT(11) NOT NULL,
        `ap_DayPrice` decimal(7, 2) NOT NULL,
        PRIMARY KEY(`ap_Id`),
		FOREIGN KEY (`ap_rId`)  REFERENCES `Room` (`r_Id`),
		FOREIGN KEY (`ap_aId`)  REFERENCES `Accommodation` (`a_Id`)
    )
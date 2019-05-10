 CREATE TABLE
    `Service` (
        `s_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `s_oId` INT(11) NOT NULL,   
        `s_pId` INT(11) NOT NULL,
        `s_apId` INT(11) NULL,
        `s_Days` INT NULL,
        `s_Price` decimal(7, 2) NOT NULL,
        PRIMARY KEY(`s_Id`),
		FOREIGN KEY (`s_oId`)  REFERENCES `Order` (`o_Id`),
		FOREIGN KEY (`s_pId`)  REFERENCES `Package` (`p_Id`),
        FOREIGN KEY (`s_apId`)  REFERENCES `AccommodationPrice` (`ap_Id`)
    ) 
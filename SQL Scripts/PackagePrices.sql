 CREATE TABLE
    `PackagePrices` (
        `pp_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `pp_pId` INT(11) NOT NULL,  
        `pp_From` INT NOT NULL,   
        `pp_Till` INT NULL,
        `pp_Price`decimal(7,2) NULL,
        PRIMARY KEY(`pp_Id`),
		FOREIGN KEY (`pp_pId`)  REFERENCES `Package` (`p_Id`)
    ) 
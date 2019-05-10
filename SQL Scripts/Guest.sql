    `Guest` (
        `g_Id` INT(11) NOT NULL AUTO_INCREMENT,
        `g_oId` INT NOT NULL,        
        `g_Name` varchar(50) NULL,
        `g_Family` varchar(100) NULL,
        PRIMARY KEY(`g_Id`),
        FOREIGN KEY (`g_oId`)  REFERENCES `order` (`o_Id`)
    )

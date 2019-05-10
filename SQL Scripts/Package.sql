 CREATE TABLE
    `Package` (
        `p_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `p_Name` varchar(50) NOT NULL,
        `p_ShortLabel` varchar(10) NOT NULL,
        `p_MinDayPrice` decimal(6, 0)  NOT NULL,
        `p_Percent` decimal(5, 2)  NOT NULL,
        `p_IsWithAcc` TINYINT NOT NULL,
        `p_Default` TINYINT NOT NULL,
        PRIMARY KEY(`p_Id`)
    )
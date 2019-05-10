 CREATE TABLE
    `Accommodation` (
        `a_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `a_Name` varchar(10) NOT NULL,   
        `a_ShortCode` varchar(10) NOT NULL,
        `a_Capacity` INT NOT NULL,
        PRIMARY KEY(`a_Id`)
    )
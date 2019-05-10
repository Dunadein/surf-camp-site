 CREATE TABLE
    `Villa` (
        `v_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `v_Name` varchar(50) NOT NULL,
        `v_Folder` varchar(50) NOT NULL,
        `v_MinPaxFor` INT  NULL,
        `v_Default` TINYINT  NOT NULL,
        `v_Enabled` TINYINT  NOT NULL,
        PRIMARY KEY(`v_Id`)
    )
 CREATE TABLE
    `PayLog` (
        `pl_Id` INT(11) NOT NULL AUTO_INCREMENT,   
        `pl_oId` INT(11) NOT NULL,  
        `pl_RequestId` varchar(200) NOT NULL,   
        `pl_EuroAmount` decimal (7,2) Not null,
        `pl_Amount` decimal (9,2) Not null,
        `pl_ToPay` decimal (9,2) Not null,
        `pl_TimeMark` datetime not NULL,
        `pl_Direction` smallint not null,
		`pl_Status` smallint not null,
        PRIMARY KEY(`pl_Id`),
		FOREIGN KEY (`pl_oId`)  REFERENCES `Order` (`o_Id`)
    ) 
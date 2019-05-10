CREATE TABLE
    `Order` (
        `o_Id` INT(11) NOT NULL AUTO_INCREMENT,
        `o_Created` datetime NOT NULL,
        `o_Hash` varchar(150) NOT NULL,
        `o_ContactEmail` varchar(150) NOT NULL,
        `o_ContactPhone` varchar(20) NOT NULL,
        `o_GuestName` varchar(50) NOT NULL,
        `o_GuestFamily` varchar(50) NOT NULL,
        `o_DateFrom` datetime NOT NULL,
        `o_DateTill` datetime NULL,
        `o_GuestsCount` INT(2) NOT NULL,
        `o_Price` decimal(6,0) Not null,
        `o_Status` smallint(2) not null,
        `o_Comment` varchar(2000) null,
        `o_Locale` varchar(10) not null,
        `o_Payed` decimal(6,2) null,
		`o_Commission` tinyint not null,
        PRIMARY KEY(`o_Id`)
    )
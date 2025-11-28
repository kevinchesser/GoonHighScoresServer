GoonHighScores.db

CREATE TABLE Character (
	Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	Name TEXT(12) NOT NULL COLLATE NOCASE,
	DiscordUserId TEXT(60)
);

CREATE UNIQUE INDEX character_name
ON Character(name);

/*
INSERT INTO Character(Name, DiscordUserId) VALUES
('Some name', 'Some id');
*/

CREATE TABLE Skill (
	Id INTEGER NOT NULL PRIMARY KEY,
	Name Text(30) NOT NULL
);

/*
INSERT INTO Skill(Id, Name) VALUES
(0, 'Overall'),
(1, 'Attack'),
(2, 'Defence'),
(3, 'Strength'),
(4, 'Hitpoints'),
(5, 'Ranged'),
(6, 'Prayer'),
(7, 'Magic'),
(8, 'Cooking'),
(9, 'Woodcutting'),
(10, 'Fletching'),
(11, 'Fishing'),
(12, 'Firemaking'),
(13, 'Crafting'),
(14, 'Smithing'),
(15, 'Mining'),
(16, 'Herblore'),
(17, 'Agility'),
(18, 'Thieving'),
(19, 'Slayer'),
(20, 'Farming'),
(21, 'Runecraft'),
(22, 'Hunter'),
(23, 'Construction'),
(24, 'Sailing');
*/



CREATE TABLE XpDrop (
	Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	CharacterId INTEGER NOT NULL,
	SkillId INTEGER NOT NULL,
	Xp INTEGER NOT NULL,
	Level INTEGER NOT NULL,
	Rank INTEGER NOT NULL,
	Timestamp TEXT(40) NOT NULL,
	CONSTRAINT SkillId_FK FOREIGN KEY (SKillId) REFERENCES Skill(Id) DEFERRABLE INITIALLY IMMEDIATE,
	CONSTRAINT CharacterId_FK FOREIGN KEY (CharacterId) REFERENCES Character(Id) DEFERRABLE INITIALLY IMMEDIATE
);

/* - Scratchpad testing queries
SELECT Xp, MAX(Id) from XpDrop
WHERE CharacterId = 1 AND SkillId = 0
Group BY Id;

SELECT MAX(Xp) from XPDrop WHERE CharacterId = 1 AND SkillId = 0;

Select xpDrop1.Xp
FROM XpDrop xpDrop1
LEFT OUTER JOIN XpDrop xpDrop2 on xpDrop1.CharacterId = xpDrop2.CharacterId
AND xpDrop1.Xp < xpDrop2.Xp;

INSERT INTO XpDrop(CharacterId, SkillId, Xp, Level, Rank, Timestamp) VALUES 
(1, 0, 150, 2, 5000, 'timestamp')

DROP Table XpDrop;


INSERT INTO XpDrop(CharacterId, SkillId, Xp, Level, Rank, Timestamp) VALUES 
(1, 0, 500000000, 40000, 2326, '2025-11-25 15:40:37.639');
INSERT INTO XpDrop(CharacterId, SkillId, Xp, Level, Rank, Timestamp) VALUES 
(4, 0, 200000000, 250000, 1992, '2024-11-25 15:40:37.639');
Select Max(Xp), SkillId from XpDrop 
WHERE CharacterId = 1
Group BY SkillId;

2025-11-24 07:44:22.537

Select XpDrop.Xp, XpDrop.SkillId, XpDrop.Timestamp, Character.Name, Character.Id as CharacterId from XpDrop INNER JOIN Character ON XpDrop.CharacterId = Character.Id
WHERE XpDrop.Timestamp > '2025-11-24 07:44:22.537' AND XpDrop.SkillId = 0

UNION ALL

Select XpDrop.Xp, XpDrop.SkillId, XpDrop.Timestamp, Character.Name, Character.Id as CharacterId from XpDrop INNER JOIN Character ON XpDrop.CharacterId = Character.Id
WHERE XpDrop.Timestamp = (
    SELECT MAX(XpDrop.Timestamp)
    FROM XpDrop
    WHERE XpDrop.Timestamp <= '2025-11-24 07:44:22.537') AND XpDrop.SkillId = 0;
    
SELECT Id from Character where Name = '';
SELECT Xp, SkillId, Timestamp, Level, Rank from XpDrop WHERE CharacterId = 1;
*/



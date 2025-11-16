GoonHighScores.db

CREATE TABLE Character (
	Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	Name TEXT(12) NOT NULL,
	DiscordUserId TEXT(60)
);

INSERT INTO Character(Name, DiscordUserId) VALUES
('Wooshylooshy', '188478537102000137');

INSERT INTO Character(Name, DiscordUserId) VALUES
('killercody07', '290340752679239682'),
('jamarc','237020028795355136');

INSERT INTO Organizer (DiscordId,Note) VALUES
	 ('188478537102000137','wooshy'),
	 ('235525388502171649','mira'),
	 ('135134371828072449','ken'),
	 ('143639064367595520','stnu'),
	 ('262084940236062720','pyre'),
	 ('245334372947984385','bz'),
	 ('245334372947984385','lilscrappy'),
	 ('200496102716211200','infi');
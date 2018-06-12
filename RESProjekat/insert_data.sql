INSERT INTO connection (one, two) VALUES (2,5);
INSERT INTO connection (one, two) VALUES (1,4);
INSERT INTO connection (one, two) VALUES (3,2);
INSERT INTO connection (one, two) VALUES (4,5);

INSERT INTO TypeTable (title) VALUES ('Default');
INSERT INTO TypeTable (title) VALUES ('Human');
INSERT INTO TypeTable (title) VALUES ('Animal');
INSERT INTO TypeTable (title) VALUES ('Alien');
INSERT INTO TypeTable (title) VALUES ('Parent');
INSERT INTO TypeTable (title) VALUES ('Object');

INSERT INTO resource (title, rname, description, type, connectedTo, connectedType) VALUES ('Male', 'Pera', 'Regular guy', 1, 4, 2);
INSERT INTO resource (title, rname, description, type, connectedTo, connectedType) VALUES ('Dog', 'Snoopy', 'Golden retriever', 2, 5, 1);
INSERT INTO resource (title, rname, description, type, connectedTo, connectedType) VALUES ('Gungan', 'Jar Jar Binks', 'Military Commander', 3, 2, 5);
-- Fix Passenger UserIds to link them to their user accounts
UPDATE Passengers SET UserId = 'd7843c0e-deb4-4f44-90af-53260165ccd6' WHERE Id = 1; -- Alice Jones
UPDATE Passengers SET UserId = '5465f747-f94e-4f0f-b3e3-8e2e942bd95e' WHERE Id = 2; -- Tina Thomas
UPDATE Passengers SET UserId = '747871f9-2597-4617-ba28-0502d7f5998b' WHERE Id = 3; -- John Lopez
UPDATE Passengers SET UserId = 'e869ddd6-c6bb-47eb-bfe2-01d303ed1d11' WHERE Id = 4; -- Wendy Moore
UPDATE Passengers SET UserId = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' WHERE Id = 5; -- Jane Doe

-- Verify the update
SELECT Id, FirstName, LastName, 
       CASE WHEN UserId IS NULL THEN 'NOT LINKED' ELSE 'LINKED' END AS LinkStatus
FROM Passengers
WHERE Id IN (1,2,3,4,5);

import random
import uuid
from datetime import datetime, timedelta

# Data Lists
first_names = ["John", "Jane", "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack", "Kelly", "Liam", "Mia", "Noah", "Olivia", "Peter", "Quinn", "Rose", "Sam", "Tina", "Uma", "Victor", "Wendy", "Xander", "Yara", "Zane"]
last_names = ["Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin"]
cities = ["New York", "London", "Paris", "Tokyo", "Dubai", "Singapore", "Los Angeles", "Sydney", "Berlin", "Rome", "Cairo", "Toronto", "Beijing", "Mumbai", "Moscow", "Istanbul", "Bangkok", "Seoul", "Madrid", "Chicago"]
aircrafts = ["Boeing 737", "Boeing 747", "Boeing 777", "Boeing 787", "Airbus A320", "Airbus A330", "Airbus A350", "Airbus A380", "Embraer 190"]
classes = ["Economy", "Business", "First Class"]
comments = ["Great flight!", "Terrible service.", "Average experience.", "Food was cold.", "Comfortable seats.", "On time.", "Delayed but handled well.", "Will fly again.", "Crew was rude.", "Smooth landing."]

def generate_phone():
    return f"+1-{random.randint(200, 999)}-{random.randint(100, 999)}-{random.randint(1000, 9999)}"

def generate_passport():
    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    digits = "0123456789"
    return random.choice(chars) + "".join(random.choices(digits, k=8))

# SQL Lists
# CLEANUP FIRST (Order matters due to foreign keys)
cleanup_sql = [
    "DELETE FROM Feedbacks;",
    "DBCC CHECKIDENT ('Feedbacks', RESEED, 0);",
    "UPDATE Seats SET BookingId = NULL;", # Break dependency
    "DELETE FROM Bookings;",
    "DBCC CHECKIDENT ('Bookings', RESEED, 0);",
    "DELETE FROM Seats;",
    "DBCC CHECKIDENT ('Seats', RESEED, 0);",
    "DELETE FROM Flights;",
    "DBCC CHECKIDENT ('Flights', RESEED, 0);",
    "DELETE FROM Passengers;",
    "DBCC CHECKIDENT ('Passengers', RESEED, 0);",
    # Only delete users that look like ours (safety mechanism)
    "DELETE FROM AspNetUsers WHERE Email LIKE '%@example.com';",
    "-- Note: Not deleting Users to preserve your Admin account. New users will be appended.",
]

users_sql = []
passengers_sql = []
flights_sql = []
seats_sql = []
bookings_sql = []
feedback_sql = []

# State for relationships
user_ids = []
flight_objects = [] # {id, seats: []}
# State for relationships
user_ids = []
flight_objects = [] # {id, seats: []}
passenger_ids = list(range(1, 26))

# 1. Users (25 Users)
# Dummy hash for "Password123!" (This is just a placeholder example hash)
password_hash = "AQAAAAEAACcQAAAAEH8J8..." 
# AspNetUsers uses GUID strings, so no IDENTITY_INSERT needed.

for i in range(25):
    uid = str(uuid.uuid4())
    user_ids.append(uid)
    fname = random.choice(first_names)
    lname = random.choice(last_names)
    username = f"{fname}{random.randint(100,999)}"
    email = f"{username}@example.com"
    
    # Minimal AspNetUsers insert
    sql = f"INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FirstName, LastName, Role) VALUES ('{uid}', '{username}', '{username.upper()}', '{email}', '{email.upper()}', 1, '{password_hash}', '{str(uuid.uuid4())}', '{str(uuid.uuid4())}', 0, 0, 0, 0, '{fname}', '{lname}', 'Customer');"
    users_sql.append(sql)

# 2. Passengers (25)
passengers_sql.append("SET IDENTITY_INSERT Passengers ON;")
for i in range(1, 26):
    fname = random.choice(first_names)
    lname = random.choice(last_names)
    email = f"{fname.lower()}.{lname.lower()}{i}@example.com" # Ensure uniqueness with ID
    phone = generate_phone()
    passport = generate_passport()
    national_id = "".join(random.choices("0123456789", k=14))
    
    # Link some to users (Strict 1:1 or NULL)
    if i == 1: # Init pool once
        user_pool = list(user_ids)
        random.shuffle(user_pool)
        
    linked_uid = user_pool.pop() if user_pool else 'NULL'
    linked_uid_val = f"'{linked_uid}'" if linked_uid != 'NULL' else "NULL"
    
    sql = f"INSERT INTO Passengers (Id, FirstName, LastName, Email, PhoneNumber, PassportNumber, NationalId, IsArchived, UserId) VALUES ({i}, '{fname}', '{lname}', '{email}', '{phone}', '{passport}', '{national_id}', 0, {linked_uid_val});"
    passengers_sql.append(sql)
passengers_sql.append("SET IDENTITY_INSERT Passengers OFF;")
# 3. Flights & Seats (25 Flights)
flights_sql.append("SET IDENTITY_INSERT Flights ON;")
seats_sql.append("SET IDENTITY_INSERT Seats ON;")
seat_global_id = 1

for i in range(1, 26):
    flight_num = f"AMS{1000 + i}" # Unique sequential flight number
    origin = random.choice(cities)
    destination = random.choice([c for c in cities if c != origin])
    start_date = datetime.now() + timedelta(days=random.randint(1, 30))
    start_date = start_date.replace(hour=random.randint(0, 23), minute=random.randint(0, 59), second=0)
    end_date = start_date + timedelta(hours=random.randint(2, 12))
    aircraft = random.choice(aircrafts)
    avail_seats = 60 # 10 rows * 6 cols = 60 seats generated below
    
    sql = f"INSERT INTO Flights (FlightId, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime, AircraftType, AvailableSeats) VALUES ({i}, '{flight_num}', '{origin}', '{destination}', '{start_date.strftime('%Y-%m-%d %H:%M:%S')}', '{end_date.strftime('%Y-%m-%d %H:%M:%S')}', '{aircraft}', {avail_seats});"
    flights_sql.append(sql)
    
    # Generate Seats for this Flight
    flight_seats = []
    rows = 10 
    cols = ['A', 'B', 'C', 'D', 'E', 'F']
    for r in range(1, rows + 1):
        for c in cols:
            seat_num = f"{r}{c}"
            seat_class = "Business" if r <= 2 else "Economy"
            price = 500 if seat_class == "Business" else 150
            
            # Add to SQL
            s_sql = f"INSERT INTO Seats (SeatId, SeatNumber, Class, FlightId, SeatPrice, IsAvailable) VALUES ({seat_global_id}, '{seat_num}', '{seat_class}', {i}, {price}, 1);" # Default available
            seats_sql.append(s_sql)
            
            flight_seats.append({'id': seat_global_id, 'num': seat_num, 'price': price})
            seat_global_id += 1
            
    flight_objects.append({'id': i, 'seats': flight_seats})

flights_sql.append("SET IDENTITY_INSERT Flights OFF;")
seats_sql.append("SET IDENTITY_INSERT Seats OFF;")
# 4. Bookings (25) - Select random flights and available seats
bookings_sql.append("SET IDENTITY_INSERT Bookings ON;")
booking_id = 1

for i in range(25):
    passenger_id = random.choice(passenger_ids)
    flight = random.choice(flight_objects)
    
    if not flight['seats']: continue # Should not happen
    
    # Pick a random seat
    seat = random.choice(flight['seats'])
    flight['seats'].remove(seat) # Remove so it's not picked again for this flight RAM-wise (approximation)
    
    # Update Seat SQL to make it unavailable
    price = seat['price']
    
    sql = f"INSERT INTO Bookings (Id, FlightId, PassengerId, SeatNumber, BookingDate, TicketPrice, Status) VALUES ({booking_id}, {flight['id']}, {passenger_id}, '{seat['num']}', '{datetime.now().strftime('%Y-%m-%d %H:%M:%S')}', {price}, 0);"
    bookings_sql.append(sql)
    
    # Update linked Seat
    update_seat = f"UPDATE Seats SET IsAvailable = 0, BookingId = {booking_id} WHERE SeatId = {seat['id']};"
    bookings_sql.append(update_seat)
    
    booking_id += 1

bookings_sql.append("SET IDENTITY_INSERT Bookings OFF;")
# 5. Feedback (25) - Reduced proportional to flights
feedback_sql.append("SET IDENTITY_INSERT Feedbacks ON;")
for i in range(1, 26):
    uid = random.choice(user_ids)
    rating = random.randint(1, 5)
    comment = random.choice(comments)
    fid = random.choice(flight_objects)['id']
    
    sql = f"INSERT INTO Feedbacks (FeedbackId, UserId, Rating, Comment, CreatedAt, FlightId) VALUES ({i}, '{uid}', {rating}, '{comment}', '{datetime.now().strftime('%Y-%m-%d %H:%M:%S')}', {fid});"
    feedback_sql.append(sql)
feedback_sql.append("SET IDENTITY_INSERT Feedbacks OFF;")

# Combine all
full_script = cleanup_sql + users_sql + passengers_sql + flights_sql + seats_sql + bookings_sql + feedback_sql

# Add a fix for Flight.AvailableSeats to match the actual Seats table
full_script.append("-- Sync Flight.AvailableSeats with actual available seats")
full_script.append("UPDATE Flights SET AvailableSeats = (SELECT COUNT(*) FROM Seats WHERE Seats.FlightId = Flights.FlightId AND Seats.IsAvailable = 1);")

# Ensure ALL users are verified as requested
full_script.append("-- Force verify all users")
full_script.append("UPDATE AspNetUsers SET EmailConfirmed = 1;")

with open("queries/seed_data.sql", "w") as f:
    f.write("\n".join(full_script))

print("Complete")

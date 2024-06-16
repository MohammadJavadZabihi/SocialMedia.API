Attention
All requests start with "http://mahanapptest.ir/api/Users/The Action you need" (for user oprations)

All requests start with 
" http://mahanapptest.ir/api/ AuthenticationForToken/The Action you need" (for user Authentication)

________________________________________Authentication
All endpoints require authentication. Make sure to include the JWT token in the Authorization header for each request.
Endpoint : POST authenticate
Description : It gives you a token that you can use to authenticate
Response : 
•	 500 Internal Server Error: Indicates a server error.
•	 200 OK: Return token api with 24 hour Expire Time
•	Unauthorized, if you don’t have permission

Example Request:
	Post http://mahanapptest.ir /api/AuthenticationForToken/authenticate	
	Content-Type: application/json
	{
  	"UserName": "username",
  	"Password": "password"
}

________________________________________
Endpoints
1. Register a New User
Endpoint: POST /RegisterUser
Description: Registers a new user in the system.
Request Body:
•	UserName (string): The username of the new user.
•	Email (string): The email of the new user.
•	Password (string): The password of the new user.
•	Bio (string): The Biography of the new user.
Response:
•	200 OK: Returns registration success information and activation link.
•	400 Bad Request: Indicates validation errors or if the username/email already exists.
•	500 Internal Server Error: Indicates a server error.
Example Request:
POST http://mahanapptest.ir/api/Users/ RegisterUser
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json

{
  "UserName": "newUser",
  "Email": "newuser@example.com",
  "Password": "Password123!",
}
________________________________________
2. Fully Update a User
Endpoint: PUT /FullUpdate
Description: Fully updates user information.
Request Body:
•	OldUserName (string): The OldUserNameof the user.
•	NewUserName (string): The NewUsername of the user.
•	NewEmail (string): The new email of the user.
•	NewBio (string): The new bio of the user.
Response:
•	200 OK: Returns the update success information.
•	400 Bad Request: Indicates validation errors.
•	404 Not Found: Indicates the user was not found.
•	500 Internal Server Error: Indicates a server error.
Example Request:
PUT http://mahanapptest.ir/api/Users/FullUpdate
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json
{
  " OldUserName ": "existingUser",
  " NewUserName ": "existingUser",

  "NewEmail": "newemail@example.com",
  "NewBio": "This is the updated bio."
}
________________________________________
3. Delete a User by Username
Endpoint: DELETE / DeletUserWithUserName
Description: Deletes a user based on the provided username.
Request Body:
•	UserName (string): The username of the user.
Response:
•	200 OK: Indicates the user was successfully deleted.
•	404 Not Found: Indicates the user was not found.
•	500 Internal Server Error: Indicates a server error.
Example Request:
DELETE http://mahanapptest.ir/api/Users/ DeletUserWithUserName
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json
{
  "UserName": "exampleUser",
}
________________________________________
4. Reset User Password
Endpoint: POST / ResetUserPassword/Forgot
Description: Resets the user's password When Forgot.
Request Parameters:
•	UserName (string): The UserName of User.
•	Password (string): The NewPassword of User.
•	Email (string): The Email of User.
Request Body:
•	UserName (string): The UserName of User.
•	Password (string): The NewPassword of User.
•	Email (string): The Email of User.
Response:
•	200 Ok: Indicates the password was successfully reset.
•	400 Bad Request: Indicates validation errors.
•	404 Not Found: Indicates the user was not found.
•	500 Internal Server Error: Indicates a server error.


Example Request:
POST http://mahanapptest.ir/api/Users/ ResetUserPassword/Forgot
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json-patch+json
{
  "UserName": "exampleUser",
  " Password ": "new Password ",
  " Email ": "example Email ",
}
________________________________________
5. Activate User Account
Endpoint: POST /ActiveUserAccount/{Id}
Description: Activates a user account using the provided active code.
Request Parameters:
•	Id (string): The active code for the user account.
Response:
•	200 OK: Indicates the account was successfully activated.
•	400 Bad Request: Indicates validation errors.
•	500 Internal Server Error: Indicates a server error.
Example Request:
POST http://mahanapptest.ir/api/Users/ActiveUserAccount/ABC123
Authorization: Bearer <JWT_TOKEN>
________________________________________
6. Login User
Endpoint: POST /LoginUser
Description: Authenticates a user and returns a JWT token if successful.
Request Body:
•	Email (string): The email of the user.
•	Password (string): The password of the user.
Response:
•	200 OK: Returns login success information and JWT token.
•	400 Bad Request: Indicates validation errors or inactive account.
•	404 Not Found: Indicates the user was not found.
Example Request:
POST http://mahanapptest.ir/api/Users/LoginUser
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json
{
  "Email": "example@example.com",
  "Password": "Password123!"
}
7. Edite user Password When User Is Login
Endpoint: POST /ResetUserPassword
Description: Authenticates a user and returns a JWT token if successful.
Request Body:
•	UserName (string): The UserNameof the user.
•	Password (string): The password of the user.
Response:
•	200 OK: Returns ResetUserPassword success information and JWT token.
•	400 Bad Request: Indicates validation errors or inactive account.
•	404 Not Found: Indicates the user was not found.
Example Request:
POST http://mahanapptest.ir/api/Users/ResetUserPassword
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json
{
  "UserName": "UserName",
  "Password": "Password123!"
}



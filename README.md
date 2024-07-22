## HotelManagementSystem

### Core functionalities

- Room `categories` can be added, viewed and removed.
- `Rooms` can be added under each categories. A category can have n number of rooms.
- `Bookings` can me made for a category. The available rooms are checked before a booking is created
- Availability of a category can be checked for a date range.
- The data is stored and retrived from `mongo db` (can be configured in the appsettings.json)
- A `global error handler` is implemented to catch all the unhandled exceptions and return a pretty response.
- Proper `validations` are added for the endpoints along with some custom attribute validations for the API endpoints.
- Proper unit tests are added for controllers and services.
- Pagination is added for get all categories API endpoint

Please see the swagger documentation for the details of the API endpoints after running the project.

### Code organization

- A `3 layer` architecture is used, of which first is a presentation layer using Web API, second is an application layer and third is a data access layer.
- Design patterns like `repository pattern` and `dependancy injection` are used to reduce the coupling of each layers.
- Interfaces are used to ensure loose coupling between the diffrent layers.

### Additional features

- A `rooms collection` has been added under categories. Multiple rooms can be added for a category. The category will be available for a date range, if any of the rooms are available.

### Enhancements

- `Caching` can be added to improve performance. For eg, get all rooms for a category.
- Enhance the create booking API, to create bookings for `multiple rooms/categories` at a time. We can even specify the number of rooms required for each category.
- API's can be added to `change booking date`, based on availability and `delete` a booking.
- Can add `filters` and `sorting` for the get endpoints.

### Setup

To run the TopupManager API, ensure you .NET Core SDK(Version 8) installed on your machine.

### Running the Application using command line

- Clone the repository.
- Navigate to the project root directory.
- Configure the database connection string and database name under the section `MongoDbSettings` in `appsettings.json`.
- Restore the dependencies using `dotnet restore`.
- Run the application using `dotnet run --project src/HotelManagementSystem.API --urls=http://localhost:5001/`, in another terminal.
- The app will be start at `http://localhost:5001`.
- Open `http://localhost:5001/swagger/index.html` to view the swagger API documentation.

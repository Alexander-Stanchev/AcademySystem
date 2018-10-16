# Pichovete-na-naroda
<br>
Group Project for the database Alpha Academy module<br><br>

# TEAM NAME: Pichovete na naroda<br>
# TEAM LEADER: Teo<br>
# TEAM MEMBERS: Ali, Stanchev<br><br>

# Topic
The topic of the project is an academy system. The system consist of 3 user types - students, teachers and admin. Each of the type has a specific set of commands, that can be used to invoke data, about what is stored for the particular user in the database.

# Database
The database used for this project is an SQL relational database with 6 different tables. 
- Users: deals with all the user types in the application. The table has a self relation with a column called "MentorId".
- Roles: a "static" table. It keeps the 3 available roles for users in the application: Admin, Teacher, Student. It has a one to many relation with the Users table.
- Courses: table that deals with the courses in the application. Also it has a many to many relation with users(enrolled courses is the associative table). It has a one to many relation with users(TeacherId field).
- Assaignments : table that keeps the assignments for the courses. Has one to many relation with users and one to many relation with Courses.
- EnrolledStudents - associative table for making the many to many relation between students and courses.
- Grades - associative table for making the many to many relation between Assigments and Users.

# Commands

| Command | Parameters | Brief Explanation
| ------ | ------ | ------ |
|Login | Username(string) and Password(string) | Command that should be used at the start of the application|
|Register | Username(string), Password(string) and Full Name | Command that register a new user with role "Student". Can only be used at start of the application. |
|CheckGradesForCourse | CourseName(string) | Command available only to students. Lists the grades that a student recieved for a certain course, that he is enrolled at. |
|EnrollStudent |CourseName(string) | Command available only to students. Enrolls the student in a new course. |
|ExportGrades |No parameters | Command available only to students. Exports the grades that the student has to a PDF file. |
|ListAvailableCourses |No parameters | Command available only to students. Shows a list of the courses that are available for enrolling into. |
|Addcourse | StartDate(datetime), EndDate(datetime), CourseName(string) | Command available only to teachers. Creates a new course. |
|Evaluatestudent | UserName(string), AssignmentID(int), Grade(int) |Command available only to teachers. Grades an assignment for the student wtih the given username.|
|ListStudents| CourseName(string) |Command available only to teachers. Lists all students enrolled in a certain course. |
|ListUsers | RoleId(int) | Command available only to administrator. Lists all users of a certain type. |
|UpdateStudentToTeacher | UserName(string) | Updates the role of a user to a teacher. |

# Design Patterns
The project implements a plethora of different design patterns all battling with a certain problem. We are using repository pattern to wrap the dbcontext created by EntityFramework. The main purpose of that is to make the application easily testable and also not to couple with EntityFramework given the possibility that at some point the framework may need to be changed. We are using adapter patterns on all .NET common library static members("Console" for example), in order to be able to easily change them and also to test them. Also service pattern and command pattern are used working as our backend(services) and frontend(commands). In that way the layers of the applications are separated well enough, so functionality can easily be attached and detached if needed.
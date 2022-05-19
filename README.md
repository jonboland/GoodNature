![Good Nature logo](GoodNature/wwwroot/images/home.png)


Good Nature is a conservation app created with ASP.NET Core MVC. It provides potential volunteers
with information about a variety of projects and lets them shortlist and select those they would 
like to work with.

## Current Features

The features implemented so far are described below.

### Unregistered Users

There are currently three types of user: unregistered, registered, and admin.

Unregistered users (and those not currently logged in) see a homepage with
several sections. 

At the top is a hero image and registration link.

Beneath the hero image there is a set cards of representing the different conservation projects:

![Good Nature logo](GoodNature/wwwroot/images/projects.png)

And towards the bottom there are *Contact Us* and *About Us* sections: 

![Good Nature logo](GoodNature/wwwroot/images/contact.png)

### Registering

Volunteers can register by clicking the register button in the center of the hero image. They can
also register via one of the conservation project cards. If the latter option is chosen,
the project described in the card will be automatically added to the users shortlist (more on this later).

![Good Nature logo](GoodNature/wwwroot/images/register.png)

### Registered Users

Registered users can click the *Login* navbar option to select projects and view project files:

![Good Nature logo](GoodNature/wwwroot/images/login.png)

Once logged in, users see a different homepage that contains the projects they have
shortlisted and applied to volunteer with. These are formatted as a collapsible list:

![Good Nature logo](GoodNature/wwwroot/images/collapsed.png)

![Good Nature logo](GoodNature/wwwroot/images/expanded.png)

And clicking an item displays the relevant content:

![Good Nature logo](GoodNature/wwwroot/images/bumblebeevid.png)

Clicking the *Choose Projects* button near the top right of the homepage takes the user
to a page where they can manage their projects. Checkboxes are used to shortlist or apply
to volunteer with projects. And once the *Save* button has been clicked, the user is
returned to the homepage where any changes are reflected.

![Good Nature logo](GoodNature/wwwroot/images/select.png)

### Admin User

The admin user has access to an additional dropdown menu that allows configuration
and management of the system:

![Good Nature logo](GoodNature/wwwroot/images/adminmenu.png)

The pages that can be accessed through this menu allow the administrator to complete tasks
such as:

- Defining which media types can be used. 
- Adding and editing new projects and content items. 
- Specifying the relationships between users and projects.

![Good Nature logo](GoodNature/wwwroot/images/adminscreens.png)

## Next Steps

Some of the features that could be added following feedback are listed below:

- Create project selection tool to help volunteers find projects that match
their interests, skills, and availability.
- Add project admin user type so projects can manage their own content 
and directly view the details of people who have applied to volunteer with them.
- Create notification system to alert projects when somebody applies to volunteer.
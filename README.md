![Good Nature logo](GoodNature/wwwroot/images/home.png)


Good Nature is a conservation app created with ASP.NET Core MVC. It provides potential volunteers
with information about a variety of projects and lets them shortlist and select those they would 
like to work with.

The features implemented so far are described below.

## Unregistered Users

There are currently three types of user: unregistered, registered, and admin.

Unregistered users (and those not currently logged in) see a homepage with
several sections. 

At the top is a hero image and registration link.

Beneath the hero image there is a set cards of representing the different conservation projects:

![Good Nature logo](GoodNature/wwwroot/images/projects.png)

And towards the bottom there are *Contact Us* and *About Us* sections. 

![Good Nature logo](GoodNature/wwwroot/images/contact.png)

## Registering

Users can register by clicking the register button in the center of the hero image. They can
also register via one of the conservation project cards. If the latter option is chosen,
the project described in the card will be automatically added to the users shortlist (more on this later).

![Good Nature logo](GoodNature/wwwroot/images/register.png)



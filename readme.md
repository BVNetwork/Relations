# Relations for Episerver #
Relations lets you create semantic connections between content in Episerver based on specified rules. The relations can be used in your templates to show additonal content in a dynamic way, showing updated related content as it is added to the site.  

## Pricing ##
This module is not free. The current price is NOK 15.000. 

This module is however free for personal and development use. For all other deployments a license is required. 

To order, send an email to `info@bvnetwork.no` with the following information: 
	 	 
* Customer Company Name 
* Partner Company Name (when delivered to customer by partner) 
* Neccessary invoicing details (contact person, address etc.) 
	 	 
EPiCode.Relations is licensed per EPiServer site. One license is required for each site that has the module installed. 

## Installation ##
Run the following command in the NuGet Package Manager Console for your web site:
```
install-package EPiCode.Relations
```
You need to add the Optimizely NuGet Feed to Visual Studio (see http://nuget.optimizely.com/feed/)

## Configuration ##

Add the module in the Startup.cs in the ConfigureServices method. Below is an example

``` 
public void ConfigureServices(IServiceCollection services)
{
...
    
	services.AddRelations();

...
}
```

By default, users with the "CmsEditors" or "CmsAdmins" role will have access to the module. You may override this with your own authorization policy:

``` 
 services.AddRelations(policy =>
            {
                policy.RequireRole("MyRole");
            });
```

## Screenshots ##
![Shows admin setup for relations](https://github.com/BVNetwork/Relations/blob/master/doc/screenshots/adminmode.png)

See the module in action:

[![ScreenShot](http://img.youtube.com/vi/qhXkiS-kS9k/0.jpg)](http://youtu.be/qhXkiS-kS9k)

https://www.youtube.com/watch?v=qhXkiS-kS9k

Relations widget in the edit UI:

![Shows admin setup for relations](https://github.com/BVNetwork/Relations/blob/master/doc/screenshots/editmode.png)

Relations shown in the template:

![Shows admin setup for relations](https://github.com/BVNetwork/Relations/blob/master/doc/screenshots/relations_template.png)


# MyFormsTreeApp

is a small, simple Windows Forms C# .NET 8.0 application that demonstrates the handling of a TreeView. Nodes can be added, deleted, renamed, and moved within the tree using drag and drop. The tree can optionally be saved as XML to a file on the local disk or through a web service to a Postgres database. The web service, along with the associated database, is hosted in a Docker container (see below).
	
![img](https://github.com/uhwgmxorg/MyFormsTreeApp/blob/master/Doc/99_1.png)

The corresponding Docker command for the web service:

`docker run --name my-xml-service-container -p 3000:3000 -p 5432:5432 -e POSTGRES_USER=xml_user -e POSTGRES_PASSWORD=password -e POSTGRES_DB=mydb -d uhwgmxorg/my-xml-service-postgresql-docker-image:1.1.0`	
	
The following features are demonstrated:

- TreeView
- Drag and drop
- DataGridView
- Web service queries
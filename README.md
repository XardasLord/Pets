# Pets
This is a RESTful API created in .NET Core. The API allows you to take care of someone's pet in some period of time when the owner can't.

# API Address
Address to API: http://www.pets.pawelkowalewicz.pl

# Documentation
Address to REST API endpoint reference documentation: http://www.pets.pawelkowalewicz.pl/swagger/

# Reference Documentation
These are the REST API endpoint reference docs.

**GET**
* GET users
* GET users/{email} 
* GET users/{email}/animals
* GET users/{email}/animals/{name}
* GET users/{email}/animals_to_care
* GET users/{email}/animals_to_care/archive
* GET users/logout
* GET animals_to_care
* GET animals_to_care/archive
* GET animals_to_care/{id}

**POST**
* POST users
* POST users/login
* POST users/{email}/animals
* POST animals_to_care/add
* POST animals_to_care/care

**PUT**
* PUT users/{email}
* PUT users/{email}/animals/{name}
* PUT animals_to_care/{id}

**DELETE**
* DELETE users/{email}
* DELETE users/{email}/animals/{name}
* DELETE animals_to_care/{id}

## GET users
Returns a list of all registered users.

### Resource Information
<table>
  <tr>
    <td>Response formats</td>
    <td>JSON</td>
  </tr>
  <tr>
    <td>Requires authentication?</td>
    <td>No</td>
  </tr>
</table>

### Parameters	
None

### Example Response
![GET users](https://github.com/XardasLord/Pets/blob/master/Documentation/GET%20users.PNG)


## GET users/{email}
Returns a user with specific e-mail address given in the URL GET request.

### Resource Information
<table>
  <tr>
    <td>Response formats</td>
    <td>JSON</td>
  </tr>
  <tr>
    <td>Requires authentication?</td>
    <td>No</td>
  </tr>
</table>

### Parameters	
None

### Example Response
![GET users](https://github.com/XardasLord/Pets/blob/master/Documentation/GET%20users-%7Bemail%7D.PNG)


## GET users/{email}/animals
Returns a list of user's animals with specific e-mail address given in the URL GET request.

### Resource Information
<table>
  <tr>
    <td>Response formats</td>
    <td>JSON</td>
  </tr>
  <tr>
    <td>Requires authentication?</td>
    <td>No</td>
  </tr>
</table>

### Parameters	
None

### Example Response
![GET users](https://github.com/XardasLord/Pets/blob/master/Documentation/GET%20users-%7Bemail%7D-animals.PNG)

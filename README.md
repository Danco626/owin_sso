## Summary  
Demos the following
- SSO between two .NET (4.7.2) MVC applications using OWIN
- Refreshing the access token when making async XHR requests from the client to a Web API controller (Controllers/DataController)


## Tenant Configuration  
1. Create a web application with the following configuration
  - **Name:** Application Uno  
  - **Callback URL:** https://localhost:44386/callback  
  - **Allowed URL:** https://localhost:44386  

2. Create web application with the following configuration
  - **Name:** Application Doc  
  - **Callback URL:** https://localhost:44389/callback  
  - **Allowed URL:** https://localhost:44389  

3. Create an API  
  - **Token Expiration:** 10
  - **Token Expiration For Browser Flows:** 10
  - **Offline Access:** enabled  

## Application Configuration
1. Update **Application Uno's** web config with the domain, client id, and client secret from **step 1** and the audience from **step 3**
2. Update **Application Dos's** web config with the domain, client id, and client secret from **step 2** and the audience from **step 3**

## Run Applications
### SSO
1. Log into Application Uno  
2. In the nav bar, click on Tokens. The id, access, and refresh token should be displayed  
3. Navigate to Application Dos and click login. A quick redirect to auth0's /authorize endpoint will occur but the user should not be prompted for credentials  
4. In the nav bar, click on Tokens. The id, access, and refresh token should be displayed  
  - Note: They do not match the tokens in Application Uno  

### Refresh Token  
1. After completing the above steps, on the Tokens screen, click the **Make request** button to trigger an XHR request to /api/data  
2. If the original access token has expired, the Data Controller will make a POST to /oauth/token to retrieve a new refresh token  



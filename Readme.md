De hieronder beschreven worker ‘Users’ is verantwoordelijk voor alle communicatie omtrent het versturen en ophalen van usergegevens.

Deze applicatie gebruikt de volgende configuratie Secrets:

RabbitMQHostname
RabbitMQUsername
RabbitMQPassword
GOOGLE_CLIENT_ID
MongoDBConnectionString

CORS: Cors is ingesteld op allow all, dit is omdat de gateway verantwoordelijk is voor het verifieren van deze policy. Prometheus stuurt deze headers mee en daardoor gaan de aanvragen anders fout.
Input HTTP endpoints

GET: /me 
Hiermee kan informatie over de huidige gebruiker worden opgehaald.
Input:
- [FromHeader] string token

PUT: /username Hiermee Kan een username worden aangepast. Bij de eerste aanpassing wordt het account van een gebruiker aangemaakt.
Input:
[FromBody] User user
[FromHeader] string token

Background Service 
Deze applicatie bevat geen background services.

Opgeslagen data Binnen de applicatie worden de volgende gegevens opgeslagen 

User
- Email (Benodigd omdat dit de enige waarde is die vanuit google geverifieerd kan worden)
- Username
- UserId (voor als er een eigen Authenticatie server wordt geimplementeerd)

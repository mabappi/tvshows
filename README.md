# tvshows
## Note
- If ElasticSearch Docker container fails to start
```
    wsl -d docker-desktop
```
Then
```
    sysctl -w vm.max_map_count=262144
```
## To run the app
- Clone the repository
### Configure Docker Network and Deploy Elastic Search
- Open PowerShell or Command prompt and Navigate to src/
- Create docker network by running 
```
    docker network create vlan
```
- Start Elastic Search Docker container
```
    docker run --name es01 --net vlan -p 9200:9200 -it docker.elastic.co/elasticsearch/elasticsearch:8.6.2
```
- Note down the password and finger print from the console. You might need to go up in your command window to find the password and finger print. 
```
----------------------------------------------------------------
-> Elasticsearch security features have been automatically configured!
-> Authentication is enabled and cluster connections are encrypted.

->  Password for the elastic user (reset with `bin/elasticsearch-reset-password -u elastic`):
  lhQpLELkjkrawaBoaz0Q

->  HTTP CA certificate SHA-256 fingerprint:
  a52dd93511e8c6045e21f16654b77c9ee0f34aea26d9f40320b531c474676228
...
----------------------------------------------------------------
```
- Edit the docker-compose.yml
- Update IndexingService and tvshows Environment Variables
```
    - FingerPrint=a52dd93511e8c6045e21f16654b77c9ee0f34aea26d9f40320b531c474676228
    - Password=lhQpLELkjkrawaBoaz0Q
```
- Run 'docker-compose build'
- Then run 'docker-compose up'

Once all the container is running Open Browser
- For Logs: Navigate to http://localhost:8003/
- For MazeConsumer Service: http://localhost:8000/
- For Tv Shows Search API: http://localhost:8001/ 
## Swagger
- Tv Show Rest Api configured with Swagger.
- To Get Tv Show List 
```
     http://localhost:8001/?pageNumber=1&pageSize=30
```

### Description of Services
#### Maze Consumer
- Scrapper to downloads Tv Shows and related cast information.
- Multi threaded. Number of thread can be configured by setting up 'NumberOfThread' in app settings or via docker-compose.
- Continues downloading Tv Shows using page number 1 till the api returns 0 tv shows.
- After downloading each page sends api request to indexer service to further process and store information to Elastic search.
- After initial download of all tv shows creates trigger for next run after 1 day.
- Idea is this service will keep downloading new information every day.

### Indexer Service
- Scable service - A thin layer before Elastic Search. 


### Notes
- Exception scenarios not been implemented. e.g.
-- If all the services are not up and running, data lost will occur. Use of messaging service like RabbitMq or Kafka will ensure durability and integraty.
-- and many others :) 
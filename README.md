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
- Update IndexingService Environment Variables
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
## 
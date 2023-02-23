# tvshows
## Note
- If ElasticSearch Docker container fails to start
'wsl -d docker-desktop'
Then
'sysctl -w vm.max_map_count=262144'

## To run the app
- Clone the repository
- Open PowerShell and Navigate to src/
- Run 'docker-compose build'
- Then run 'docker-compose up'

Once all the container is running Open Browser
- For Logs: Navigate to http://localhost:8003/
- For Kibana: Navigate to http://localhost:5601/
- For MazeConsumer Service: http://localhost:8000/
- For Tv Shows Search API: http://localhost:8001/ 
## 
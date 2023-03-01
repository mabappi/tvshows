Feature: TvShowApi
Get Paginated list of TvShows including casts. Casts should be ordered by birthday

Scenario: Get Paginated List of Tv Shows
	Given TvShow Api is up and running
	When API Endpoint is called with page number 0 and page size 10
	Then Should return empty list
	When API Endpoint is called with page number 1 and page size 0
	Then Should return empty list
	When API Endpoint is called with page number 0 and page size 0
	Then Should return empty list
	When API Endpoint is called with page number 1 and page size 10
	Then Should return 10 Tv shows
	And Cast should be ordered by birthday
	When API Endpoint is called with page number 10 and page size 10
	Then Should return 10 Tv shows
	And Cast should be ordered by birthday
	When API Endpoint is called with page number 999999 and page size 50
	Then Should return empty list


	
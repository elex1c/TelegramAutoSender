# Telegram Auto Sender
## Application description
This application is a simple script for sending messages to Telegram groups. The messaging is done using the APIs of your accounts to the specified groups. The process works as follows: an account sends messages to the specified groups until it gets banned. If an account gets banned, the next one on the list is used.
## Set up
### Additional files in the root folder 
This folder has to contain three additional files: appsettings.json, ApiData.txt, GroupsSource.txt
> **Note:** You can specify your own path to ApiData.txt and GroupsSource.txt in appsettings.json. Also, you can change their names.
#### appsettings.json
Contains .txt file paths.
#### ApiDataPath (ApiData.txt)
This file contains API data in format: **APP_ID;API_HASH;SESSION_PATH**
> **Note:** APP_ID and API_HASH you can get on https://my.telegram.org/apps
> 
> **Note:** SESSION_PATH should be absolute regarding to your device
#### TelegramGroupsPath (GroupsSource.txt)
Contains groups usernames/links in format: 

If link: https://t.me/group_link

If username: @groupname

**One group per line**

## Output
It shows APIs valid lines.

![image](https://github.com/user-attachments/assets/ce51d710-cc6c-407e-9a52-af0e284c8f96)

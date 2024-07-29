# Telegram Auto Sender
## Application description
This application is a simple script for sending messages to Telegram groups. The messaging is done using the APIs of your accounts to the specified groups. The process works as follows: an account sends messages to the specified groups until it gets banned. If an account gets banned, the next one on the list is used.
## Set up
### Additional files in the root folder 
This folder has to contain three additional files: `appsettings.json`, `ApiData.txt`, `GroupsSource.txt`
> **Note:** You can specify your own path to `ApiData.txt` and `GroupsSource.txt` in `appsettings.json`. Additionally, you can change their names.
#### `appsettings.json`
Contains the paths to the `.txt` files.
#### ApiDataPath (`ApiData.txt`)
This file contains API data in format: **APP_ID;API_HASH;SESSION_PATH**
> **Note:** APP_ID and API_HASH you can get on https://my.telegram.org/apps
> 
> **Note:** The SESSION_PATH should be absolute with respect to your device.
#### TelegramGroupsPath (`GroupsSource.txt`)
Contains groups usernames/links in format: 

    https://t.me/group_link
    @groupname

**One group per line**

## Output
The output shows valid API lines.

![image](https://github.com/user-attachments/assets/ce51d710-cc6c-407e-9a52-af0e284c8f96)

It also shows how many telegram groups were read successfully.
> This does not necessarily mean these groups exist.
> 
![image](https://github.com/user-attachments/assets/9f492e08-dc47-4c4a-b087-e3eff6f7d956)

This output appears when sending ends. "Alive accounts" means they did not get banned.

![image](https://github.com/user-attachments/assets/adc90994-cc11-4a94-ad94-d273543ca51d)

Also, at the end, if some accounts get banned, it prints their UserIds

![image](https://github.com/user-attachments/assets/860b51e5-ca25-4dee-a3ca-66131dd8e418)




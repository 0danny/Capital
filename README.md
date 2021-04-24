<p align="center">
  <img src="https://i.imgur.com/4wbrHzb.png" width="350" title="Title Image">
</p>

# Capital - Stock Checker

Still under development.

<p>
Capital is a stock checking program for websites that operates in the background of your computer. It will include advanced features to scrape websites
for product data and route them to the dashboard for ease of use. It will be able to constantly ping 100s of product websites at once to check for available stock, this may include
PS5, 3080 etc. The insipiration stems from other stock checking websites were they may charge upwards of $30-40 USD to use cloud servers for this matter, however
for the average person I wanted to create a free alternative to allow anyone to contribute.

It is written in C# WPF purely for UI/UX experiences to be enjoyable and to have performance increases when updating a large amount of UI controls
at any given time. I am not using MVVM at this time as I believe a application of this size/nature it is a bit overkill for, however if it does scale
I may consider it in the future.
</p>



## How does it work?
<p>
One of the ways I plan to make this application self-sustainable is through a configuration system. This structure will allow Capital users to create and share
their own configurations with others who may want to stock check the same websites. 

<img src="https://i.imgur.com/Y5MZGnv.png" width="850">

Above is the configuration panel were they are displayed, 

<img src="https://i.imgur.com/WoFsOez.png" width="250">

Upon clicking create new this dialog will appear, in simple terms Capital will take a name, author name, and website name all for meta data. It will then ask for success keys, 
these keys will be used when Capital accesses the website in order to verify whether stock is imminent or not. For e.g. Newegg's success keys might be "In Stock" and when the product
eventually restocks and Capital pings it again, it will match the success keys and either send a notification to your phone via a Discord Webhook or a desktop notification if your available
at the time.

These configurations are available at "\Configurations\ConfigName.config"

</p>

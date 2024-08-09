# GrafanAlarmTUI
A simple terminal user interface for monitoring Grafana alerts.

## Configuration

There is a configuration file that needs to be created.  The `configuration.sample.json` file is a template for the configuration file.  The configuration file should be placed in the same directory as the GrafanaAlarmTUI executable.  By default, the program will read configuration.json in the working directory.  

## Command Line Arguments

The following command line arguments are available:

- `-c` or `--config` - Specify the configuration file to use.  If not specified, the program will look for `configuration.json` in the working directory.
- `-h` or `--help` - Display the help message.

## Exiting

To exit the program, press `Ctrl-C`.


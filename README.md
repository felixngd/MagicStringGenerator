## Feature

- One click automatically generate C# constant string classes.
- A solution for archieving zero allocation of magic string in Addressables and Localization.
- Optional integrate with Odin Inspector.

## Setup
Use [UPMGitExtension](https://github.com/mob-sakai/UpmGitExtension) to install from git tag.
Get the lastest version of `com.wolffun.codegen.magicstring`

## How to use?

1. Open the Editor tool by Tools>CodeGen>Magic String Generator

<img width="420" alt="image" src="https://user-images.githubusercontent.com/58353771/180696609-4730c65d-7d8b-4358-8d88-e9c634b63fe9.png">

2. Set a namespace, class name and file name for the generated C# file. Pick a folder to save the file.
3. Now you can access these strings from code by syntax `ClassName.GroupName.Key`. For example `AddressableKey.TexturesAvartar._48Guild`

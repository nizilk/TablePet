from openai import OpenAI
import json
from pathlib import Path
import time
import configparser


client = OpenAI(
    # defaults to os.environ.get("OPENAI_API_KEY")
    api_key="sk-WM2GGtnoub7lHpmix4ihZX0Si8K1Go5vqyXQDjIwdD3dZdeZ",
    base_url="https://api.chatanywhere.tech/v1"
    # base_url="https://api.chatanywhere.org/v1"
)


class ChatGPT:
    def __init__(self):
        # 前置内容-设定角色
        path = 'D:\Documents\GitHub\TablePet\TablePet.Services\Python\prompt.json'
        with open(path, encoding='UTF-8') as prompt_file:
            file_contents = prompt_file.read()
            psj = json.loads(file_contents)
            self.messages = psj["start"]
            self.intents = psj["intent"]

    def ask_gpt(self, question):
        # 记录问题
        self.messages.append({"role": "user", "content": question})
        # 发送请求
        rsp = client.chat.completions.create(
            model="gpt-4o-mini",    # gpt-3.5-turbo / gpt-4o-mini
            messages=self.messages
        )
        answer = rsp.choices[0].message.content
        # 记录回答
        self.messages.append({"role": "assistant", "content": answer})
        return answer
    
    def query_rec(self, question):
        self.intents.append({"role": "user", "content": question})
        self.messages.append({"role": "user", "content": question})
        rsp = client.chat.completions.create(
            model = "gpt-4o-mini",
            messages=self.intents
        )
        answer = rsp.choices[0].message.content
        del self.intents[-1]
        self.messages.append({"role": "assistant", "content": answer})
        return answer


if __name__ == '__main__':
    chat = ChatGPT()
    while(1):
        qst = input()
        print(chat.query_rec(qst))

    # print(chat.ask_gpt("请你向我打招呼。"))
    # print(chat.ask_gpt("复述上一次回答。"))
    # print(chat.ask_gpt("凯尔希，上一条我给你的指示是什么？"))



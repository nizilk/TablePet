from openai import OpenAI
import json


def gpt_35_api_stream(messages: list):
    """为提供的对话消息创建新的回答 (流式传输)

    Args:
        messages (list): 完整的对话消息
    """
    stream = client.chat.completions.create(
        model='gpt-3.5-turbo',
        messages=messages,
        stream=True,
    )
    for chunk in stream:
        if chunk.choices[0].delta.content is not None:
            print(chunk.choices[0].delta.content, end="")


def test():
    client = OpenAI(
        # defaults to os.environ.get("OPENAI_API_KEY")
        api_key="sk-WM2GGtnoub7lHpmix4ihZX0Si8K1Go5vqyXQDjIwdD3dZdeZ",
        base_url="https://api.chatanywhere.tech/v1"
        # base_url="https://api.chatanywhere.org/v1"
    )
    with open('D:\Documents\GitHub\TablePet\TablePet.Services\Python\prompt.json', encoding='UTF-8') as prompt_file:
        file_contents = prompt_file.read()
    psj = json.loads(file_contents)
    messages = [psj["start"][1]]
    completion = client.chat.completions.create(model="gpt-3.5-turbo", messages=messages)
    print(completion.choices[0].message.content)
    return completion.choices[0].message.content


if __name__ == '__main__':
    test()

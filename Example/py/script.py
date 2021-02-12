import json

def main():
	print("hello world")

	print("read json config:")

	json_path = "../ToolStripExample.json"
	config = json.load(open(json_path))
	json.dump(config, open("config.json", "w"))
	print(config)

	print("doing some work...")

	print("done")

if __name__ == "__main__":
	main()
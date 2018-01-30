using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Transmission {

	public static string incomingTransmission = "Incoming transmission...";
	public static string transmissionLost = "Transmission lost.";
	public static string transmissionComplete = "Transmission finished.";
	public static string streamingAudioTransmission = "Streaming audio transmission...";
	public static string transmissionDecoded = "Transmission decoded!\nPress Enter to continue.";

	//static Regex paraBreak = new Regex(@"(?:\n|\r)+");
	static Regex paraBreak = new Regex(@"(?:\n|\r)+#(?:\n|\r)*");
	static char[] trimChars = new char[]{"\r"[0], "\n"[0]};
	static char[] brackets = new char[]{"["[0], "]"[0]};

	public string metaData;
	public TransmissionNode[] nodes;

	public static Transmission ParseMessageText(string messageText) {
		Transmission transmission = new Transmission();
		string[] lines = paraBreak.Split(messageText);
		transmission.metaData = lines[0].Trim(trimChars);
		TransmissionNode[] nodes = new TransmissionNode[lines.Length - 1];
		for (int i = 0; i < lines.Length - 1; i++) {
			nodes[i] = ParseMessageLine(lines[i + 1]);
		}
		transmission.nodes = nodes;
		return transmission;
	}

	public static TransmissionNode ParseMessageLine(string line) {
		TransmissionNode node = new TransmissionNode();
		string[] bracketSplit = line.Split(brackets);
		if (bracketSplit.Length > 1) {
			node.portrait = bracketSplit[1];
			node.text = bracketSplit[2].Trim(trimChars);
			node.hasPortrait = true;
		} else {
			node.portrait = null;
			node.text = bracketSplit[0].Trim(trimChars);
			node.hasPortrait = false;
		}
		//Hardcoding portraits off:
		node.hasPortrait = false;
		return node;
	}

	public class TransmissionNode {
		public string text;
		public string portrait;
		public bool hasPortrait;


		public override string ToString() {
			if (hasPortrait) {
				return "[Portrait: " + portrait + "] " + text;
			} else {
				return text + "";
			}
		}
	}


		
}

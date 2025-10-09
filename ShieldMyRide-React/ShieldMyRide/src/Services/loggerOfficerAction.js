export const logOfficerAction = async (actionType, targetId, remarks = "", status = "completed") => {
  try {
    const officerId = localStorage.getItem("userId");
    if (!officerId) return;

    const payload = {
      officerId: officerId.toString(),
      actionType,
      targetId: targetId.toString(),
      remarks,
      status
    };

    const response = await fetch("https://localhost:7153/api/OfficerAssignment/create", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });

    if (!response.ok) {
      const text = await response.text();
      console.error("Failed to log officer action:", text);
    }
  } catch (error) {
    console.error("Failed to log officer action:", error);
  }
};

export interface Interaction {
  id: number;
  leadClientId: number;
  channel: string;
  summary: string;
  interactionAt: string;
  nextFollowUpAt?: string | null;
}

export interface LeadClient {
  id: number;
  fullName: string;
  phone: string;
  email?: string | null;
  source: string;
  status: string;
  nextFollowUpAt?: string | null;
  notes?: string | null;
  createdAt: string;
  updatedAt: string;
  interactions: Interaction[];
}

export interface CreateLeadClientRequest {
  fullName: string;
  phone: string;
  email?: string | null;
  source: string;
  status: string;
  nextFollowUpAt?: string | null;
  notes?: string | null;
}

export interface UpdateLeadClientRequest extends CreateLeadClientRequest {}

export interface CreateInteractionRequest {
  channel: string;
  summary: string;
  interactionAt: string;
  nextFollowUpAt?: string | null;
}

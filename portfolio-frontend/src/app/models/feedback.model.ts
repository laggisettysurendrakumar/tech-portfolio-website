export interface feedback {
  id?: number;
  relationship: string;
  communicationRating: number;
  collaborationRating: number;
  technicalSkillRating: number;
  codeQualityRating: number;
  helpfulnessRating: number;
  whatWentWell?: string;
  whatCouldBeImproved?: string;
  farewellNote?: string;
  yourName: string;
}
